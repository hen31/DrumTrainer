using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrumTrainer.Core.Entities
{
    public class Note
    {
        public int Beat { get; set; }
        public int Position { get; set; }
        public NoteType NoteType { get; set; }
        public Drum Drum { get; set; }


        public static int[] GetPosisitionsInThirthySecond(Note note)
        {
            return (GetPosisitionsInThirthySecond(note.Position, note.NoteType));
        }


        public static int[] GetPosisitionsInThirthySecond(int position , NoteType noteType)
        {
            if (noteType == NoteType.ThirtySecond)
            {
                return new int[] { position };
            }
            else if (noteType == NoteType.Sixteenth)
            {
                int posistionInThirtySecond = ((position - 1) * 2) + 1;

                return new int[] {
               posistionInThirtySecond,
               posistionInThirtySecond + 1 };
            }
            else if (noteType == NoteType.Eight)
            {
                int posistionInThirtySecond = ((position - 1) * 4) + 1;

                return new int[]{
               posistionInThirtySecond,
               posistionInThirtySecond + 1,
               posistionInThirtySecond + 2,
               posistionInThirtySecond + 3};
            }
            else if (noteType == NoteType.Quater)
            {
                int posistionInThirtySecond = ((position - 1) * 8) + 1;

                return new int[]{
                posistionInThirtySecond,
                posistionInThirtySecond + 1,
                posistionInThirtySecond + 2,
                posistionInThirtySecond + 3,
                posistionInThirtySecond + 4,
                posistionInThirtySecond + 5,
                posistionInThirtySecond + 6,
                posistionInThirtySecond + 7 };
            }
            return new int[0];
        }


        public bool IsLowestNote(Note note, MusicMeasure measure)
        {
            return measure.Notes.Where(b => b.Beat == note.Beat && b.Position == note.Position).Min(b => b.Drum) == note.Drum;

        }

        public static bool IsTopMostNote(Note note, MusicMeasure measure)
        {
          /*  if (note.Drum == Drum.ClosedHihat)
            {
                return true;
            }
            else
            {*/
               return measure.Notes.Where(b => b.Beat == note.Beat && b.Position == note.Position).Max(b=> b.Drum) == note.Drum;
         // }
        }

        public static bool IsLastNoteOfBeat(Note note, MusicMeasure measure)
        {
            return Note.GetPosisitionsInThirthySecond(note).Contains(8);
            //return measure.Notes.Where(b => b.Beat == note.Beat).OrderBy(b => Note.GetPosisitionsInThirthySecond(b).First()).ToList().Last() == note;
        }
    }
}
