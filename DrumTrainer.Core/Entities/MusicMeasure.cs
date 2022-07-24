using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace DrumTrainer.Core.Entities
{
    public class MusicMeasure
    {

        public int Measure { get; set; } = 4;

        public int RepeatCount { get; set; } = 1;

        private ObservableCollection<Note> _notes;
        public MusicMeasure()
        {
            _notes = new ObservableCollection<Note>();
        }

        public ObservableCollection<Note> Notes
        {
            get
            {
                return _notes;
            }
            set
            {
                _notes = value;
            }
        }

        public MusicMeasure AddNote(Note note)
        {


            //var positionsTakenIn32 = sameDrumsInBeat.Select(b=> b.Position)

            //sameDrumsInBeat.Select(b=> b.NoteType)


            _notes.Add(note);
            return this;
        }

        public bool CanAddNoteAtPosisition(Note note)
        {
   
            var sameDrumsInBeat = _notes.Where(b => b.Drum == note.Drum && b.Beat == note.Beat);

            bool alreadyInNotes = sameDrumsInBeat.Any(b => b.Position == note.Position && b.NoteType == note.NoteType);

            if(alreadyInNotes)
            {
                return false;
            }

            double singleNoteValue = 1f / (double)note.NoteType;

            bool valueFits = sameDrumsInBeat.Sum(b => 1f / (double)note.NoteType) + singleNoteValue <= 1f;
            if (!valueFits)
            {
                return false;
            }

            List<int> positionsTaken = new List<int>(sameDrumsInBeat.SelectMany(b => Note.GetPosisitionsInThirthySecond(b)));

            var posistions = Note.GetPosisitionsInThirthySecond(note);
            bool positionTaken = posistions.Any(b => positionsTaken.Contains(b));
            if (positionTaken)
            {
                return false;
            }

            return true;
        }

        public void RemoveNote(Note currentNote)
        {
            _notes.Remove(currentNote);
        }

       
    }
}
