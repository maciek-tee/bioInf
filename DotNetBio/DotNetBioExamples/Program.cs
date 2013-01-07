using System.Linq;
using System.Text;
using Bio.Web.Blast;

namespace DotNetBioExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            var firstFile = @"..\..\TestData\first_file.fasta";
            var secondFile = @"..\..\TestData\second_file.fasta";
            NetBioExamples.DiffSeq(firstFile, secondFile);
            NetBioExamples.ConcatenateSequences(firstFile, secondFile);
            NetBioExamples.StripNonAlphabets(firstFile);
        }
    }
}
