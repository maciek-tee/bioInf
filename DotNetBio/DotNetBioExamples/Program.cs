using System.Linq;
using System.Text;
using Bio.IO;
using Bio.Web.Blast;

namespace DotNetBioExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            const string firstFastaFilePath = @"..\..\TestData\first_file.fasta";
            const string secondFastaFilePath = @"..\..\TestData\second_file.fasta";

            // wyszukanie różnic w sekwencjach - nie działa poprawnie.
            NetBioExamples.FindDifferencesInSequences(firstFastaFilePath, secondFastaFilePath);
            
            // łączenie sekwencji podanych w dwu plikach i zapisanie do trzeciego
            NetBioExamples.ConcatenateSequences(firstFastaFilePath, secondFastaFilePath, "concatenated.fasta");
            
            NetBioExamples.StripNonAlphabets(firstFastaFilePath, "cleaned.fasta");

            // nie działa z powodu błędów w SequenceFormatters. Możliwa jest konwersja z Fasta do Fasta...
            NetBioExamples.ConvertFromOneFormatToAnother(firstFastaFilePath, "output.genbank", SequenceFormatters.GenBank);
        }
    }
}
