using DrumTrainer.Core.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DrumTrainer.ViewModels
{
    internal class ExpectedNotesPerBeat
    {

        List<BeatNote> _notesForBeat;

        public ObservableCollection<NotesPerPosition> NotesPerPosistions { get; private set; } = new ObservableCollection<NotesPerPosition>();

        public List<BeatNote> NotesForBeat
        {
            get
            {
                return _notesForBeat;
            }
            set
            {
                _notesForBeat = value;
                NotesPerPosistions.Clear();
                _notesForBeat.ToList().GroupBy(b => Note.GetPosisitionsInThirthySecond(b.ExpectedNote.Note)[0])
                    .ToList()
                    .ForEach(b => NotesPerPosistions.Add(new NotesPerPosition()
                    {
                        PositionIn32 = b.Key,
                        Notes = b.ToList()

                    }));
            }
        }
        /*  NotesPerPosistions = new ObservableCollection<NotesPerPosition>()
                    {
                        NotesForPosistion = b.ToList().GroupBy(b => Note.GetPosisitionsInThirthySecond(b.Note)[0]).ToList()
                    }*/
        public int Beat { get; internal set; }
    }
}