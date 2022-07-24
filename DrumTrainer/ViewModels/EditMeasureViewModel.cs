using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using DrumTrainer.Core.Entities;

namespace DrumTrainer.ViewModels
{
    public class EditMeasureViewModel : ViewModel
    {
        private readonly MusicMeasure measure;
        private readonly IChildView _view;

        public EditMeasureViewModel(MusicMeasure measure, IChildView view)
        {
            this.measure = measure;
            _view = view;
            Beats = new ObservableCollection<EditableBeat>();
            for (int i = 1; i <= measure.Measure; i++)
            {
                NoteType type = NoteType.Quater;
                var firstNote = measure.Notes.Where(b => b.Beat == i).FirstOrDefault();
                if (firstNote != null)
                {
                    type = firstNote.NoteType;
                }
                Beats.Add(new EditableBeat(i, type, measure));
            }


            CancelCommand = new RelayCommand(CancelEditExecute);
            SaveCommand = new RelayCommand(SaveExecute);
        }

        private void SaveExecute(object obj)
        {
            _view.Close(Beats);
        }

        private void CancelEditExecute(object obj)
        {
            _view.Close(null);
        }

        public RelayCommand CancelCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }

        public ObservableCollection<EditableBeat> Beats { get; set; }


    }

    public class EditableBeat : ViewModel
    {
        public EditableBeat(int i, NoteType noteType, MusicMeasure measure)
        {
            Beat = i;
            NotesForDrum = new ObservableCollection<NotesForDrum>();
            NoteType = noteType;
            foreach (Drum drum in Enum.GetValues(typeof(Drum)))
            {
                NotesForDrum.Add(new NotesForDrum() { Drum = drum });
                NotesForDrum.Last().ChangeToNoteType(noteType);
                NotesForDrum.Last().SyncFromMeasure(measure.Notes.Where(b => b.Beat == i && b.Drum == drum).ToList());
            }

        }
        public int Beat { get; set; }

        public ObservableCollection<NotesForDrum> NotesForDrum { get; set; }
        NoteType _noteType;
        public NoteType NoteType
        {
            get
            {
                return _noteType;
            }
            set
            {
                _noteType = value;
                OnPropertyChanged();
                ChangeToNoteType(value);


            }
        }
        internal void ChangeToNoteType(NoteType value)
        {
            foreach (var noteToDrum in NotesForDrum)
            {
                noteToDrum.ChangeToNoteType(value);
            }
        }
    }


    public class NotesForDrum : ViewModel
    {
        public NotesForDrum()
        {
            Notes = new ObservableCollection<EditableNote>();
        }
        public Drum Drum { get; set; }
        public ObservableCollection<EditableNote> Notes { get; private set; }

        public void SyncFromMeasure(List<Note> notes)
        {
            foreach (var editableNote in Notes)
            {
                editableNote.HitOnNote = false;
            }
            foreach (var note in notes)
            {
                var positions = Note.GetPosisitionsInThirthySecond(note);
                foreach (var editableNote in Notes)
                {
                    var positionsOfEditableNote = Note.GetPosisitionsInThirthySecond(editableNote.Position, NoteType);
                    if (positions.Any(b => positionsOfEditableNote.Contains(b)))
                    {
                        editableNote.HitOnNote = true;
                    }

                }
            }
        }

        NoteType _noteType;
        public NoteType NoteType
        {
            get
            {
                return _noteType;
            }
            set
            {
                _noteType = value;
                OnPropertyChanged();

            }
        }

        internal void ChangeToNoteType(NoteType noteType)
        {
            NoteType = noteType;
            Notes.Clear();
            for (int i = 1; i <= ((int)noteType); i++)
            {
                Notes.Add(new EditableNote() { Position = i });
            }
        }
    }

    public class EditableNote
    {
        public int Position { get; set; }
        public bool HitOnNote { get; set; }
    }
}
