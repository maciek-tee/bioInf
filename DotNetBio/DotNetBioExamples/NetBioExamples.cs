using System;
using System.Collections.Generic;
using Bio;
using Bio.Algorithms.Kmer;
using Bio.IO;
using Bio.Util;

namespace DotNetBioExamples
{
    public class NetBioExamples
    {
        public static void DiffSeq(string firstFile, string secondFile)
        {
            // parsowanie pierwszej listy
            if(!SequenceParsers.IsFasta(firstFile))
            {
                Console.WriteLine("Nieprawidlowy format pierwszego pliku!");
                return;
            }

            if (!SequenceParsers.IsFasta(secondFile))
            {
                Console.WriteLine("Nieprawidlowy format drugiego pliku!");
                return;
            }

            var firstParser = SequenceParsers.FindParserByFileName(firstFile);
            firstParser.Alphabet = AmbiguousProteinAlphabet.Instance;
            var firstSequenceList = firstParser.Parse();
            var firstFileSequences = Helper.ConvertIenumerableToList(firstSequenceList);

            // parsowanie drugiej listy
            var secondParser = SequenceParsers.FindParserByFileName(firstFile);
            secondParser.Alphabet = AmbiguousProteinAlphabet.Instance;
            var secondSequenceList = secondParser.Parse();
            var secondFileSequences = Helper.ConvertIenumerableToList(secondSequenceList);

            // pobranie listy KMER'ów
            var kmerBuilder = new SequenceToKmerBuilder();
            var kmerList = kmerBuilder.Build(firstFileSequences.First(), 2);
            var nodes = WordMatch.BuildMatchTable(kmerList, secondFileSequences.First(), 2);

            var list2 = new List<WordMatch>(nodes);

            var matchList = WordMatch.GetMinimalList(list2, 2);

            var list3 = new List<WordMatch>(matchList);

            // znajdŸ ró¿nice miêdzy wêz³ami
            var diffNode = DifferenceNode.BuildDiffList(list3, firstFileSequences.First(), secondFileSequences.First());

            var list4 = new List<DifferenceNode>(diffNode);

            var features = DifferenceNode.OutputDiffList(list4, firstFileSequences.First(), secondFileSequences.First());

            foreach (var compareFeature in features)
            {
                Console.WriteLine(compareFeature.Feature);
            }

        }   

        public static void ConcatenateSequences(string firstFile, string secondFile)
        {
            if (!SequenceParsers.IsFasta(firstFile))
            {
                Console.WriteLine("Nieprawidlowy format pierwszego pliku!");
                return;
            }

            if (!SequenceParsers.IsFasta(secondFile))
            {
                Console.WriteLine("Nieprawidlowy format drugiego pliku!");
                return;
            }

            var firstParser = SequenceParsers.FindParserByFileName(firstFile);
            firstParser.Alphabet = AmbiguousProteinAlphabet.Instance;
            var firstSequenceList = firstParser.Parse();
            var firstFileSequences = Helper.ConvertIenumerableToList(firstSequenceList);

            // parsowanie drugiej listy
            var secondParser = SequenceParsers.FindParserByFileName(firstFile);
            secondParser.Alphabet = AmbiguousProteinAlphabet.Instance;
            var secondSequenceList = secondParser.Parse();
            var secondFileSequences = Helper.ConvertIenumerableToList(secondSequenceList);

            // sprawdzenie czy wszystkie sekwencje wykorzystuj¹ ten sam alfabet
            IAlphabet alphabet = DnaAlphabet.Instance;
            var selectedAlphabet = false;

            foreach (var sequence in firstFileSequences)
            {
                if(!selectedAlphabet)
                {
                    alphabet = sequence.Alphabet;
                    selectedAlphabet = true;
                }
                else if(sequence.Alphabet != alphabet)
                {
                    Console.WriteLine("Niedopasowane alfabety: " + alphabet.Name + ", " + sequence.Alphabet.Name);

                    return;
                }
            }

            foreach (var sequence in secondFileSequences)
            {
                if (!selectedAlphabet)
                {
                    alphabet = sequence.Alphabet;
                    selectedAlphabet = true;
                }
                else if (sequence.Alphabet != alphabet)
                {
                    Console.WriteLine("Niedopasowane alfabety: " + alphabet.Name + ", " + sequence.Alphabet.Name);

                    return;
                }
            }

            Console.WriteLine("Uzywany alfabet: " + alphabet.Name);

            var outputFile = "concatenated.fasta";

            var firstListConcatenated = "";
            var secondListConcatenated = "";

            foreach (var sequence in firstFileSequences)
            {
                firstListConcatenated += Helper.ConvertSequenceToString(sequence);
            }

            foreach (var sequence in secondFileSequences)
            {
                secondListConcatenated += Helper.ConvertSequenceToString(sequence);
            }

            var final = firstListConcatenated + secondListConcatenated;

            var concatenatedSequence = new Sequence(alphabet, final);
            concatenatedSequence.ID = "Concatenated sequence";
            
            var formatter = SequenceFormatters.FindFormatterByFileName(outputFile);
            formatter.Write(concatenatedSequence);
            formatter.Dispose();
        }

        public static void StripNonAlphabets(string inputFile)
        {
            var parser = SequenceParsers.FindParserByFileName(inputFile);
            var sequenceList = parser.Parse();
            var sequences = Helper.ConvertIenumerableToList(sequenceList);

            var newSequences = new List<Sequence>();

            foreach (var sequence in sequences)
            {
                var sequenceChanged = false;
                var sequenceString = Helper.ConvertSequenceToString(sequence);
                var newSequence = "";
                foreach (var character in sequenceString.ToCharArray())
                {
                    if(char.IsLetter(character) || char.IsSeparator(character))
                    {
                        newSequence += character;
                    }
                    else
                    {
                        sequenceChanged = true;
                    }
                }

                // usuniêcie starszego alfabetu
                var seq = new Sequence(sequence.Alphabet, newSequence);
                newSequences.Add(seq);
            }

            // zapisanie sekwencji do pliku
            var outputFile = "cleaned.fasta";
            var formatter = SequenceFormatters.FindFormatterByFileName(outputFile);

            foreach (var sequence in newSequences)
            {
                formatter.Write(sequence);
            }
            
            formatter.Dispose();
        }

    }
}