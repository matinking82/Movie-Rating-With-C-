using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRating
{
    public static class Movie
    {
        public static List<string> GetMovies()
        {

            List<string> Movies = GetMoviesFromFile();

            Movies.ShowConsole();

            Movies = Movies.UnionMovies(GetMoviesConsole(Movies.Count));

            return Movies;
        }

        #region GetMovies
        private static List<string> UnionMovies(this List<string> movies, IEnumerable<string> newMovies)
        {

            foreach (var item in newMovies)
            {
                if (!movies.Select(m => m.ToLower()).Contains(item.ToLower()))
                {
                    movies.Add(item);
                }
            }
            return movies;
        }

        private static List<string> GetMoviesConsole(int fromFileCount)
        {
            List<string> Movies = new List<string>();
            string MovieIn;

            while (true)
            {

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Enter Movie or Series Name[");

                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(Movies.Count + 1 + fromFileCount);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("] : ");

                Console.ForegroundColor = ConsoleColor.Cyan;
                MovieIn = Console.ReadLine();
                Console.ResetColor();

                if (String.IsNullOrWhiteSpace(MovieIn))
                {
                    continue;
                }
                if (MovieIn == "done")
                {
                    break;
                }
                if (Movies.Select(m => m.ToLower()).Contains(MovieIn.ToLower()))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The Name Is Already Exists!!");
                    Console.ResetColor();

                    continue;
                }
                Movies.Add(MovieIn);
            }

            return Movies;
        }

        private static List<string> GetMoviesFromFile()
        {
            string FileName = "Movies.txt";

            StreamReader reader = new StreamReader(FileName);

            var Movies = reader
                .ReadToEnd()
                .Split("\n")
                .Select(m => m.Trim());

            List<string> Data = new List<string>();

            foreach (var item in Movies)
            {
                if (!Data.Select(d => d.ToLower()).Contains(item.ToLower()))
                {
                    Data.Add(item);
                }
            }

            return Data;
        }
        #endregion

        private static void ShowConsole(this IEnumerable<string> value)
        {
            for (int i = 0; i < value.Count(); i++)
            {
                var item = value.ElementAt(i);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(i + 1);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("] - ");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(item);
            }

            Console.ResetColor();
            Console.WriteLine("---------- From File ----------\n");
        }

        public static List<string> SortMovies(this List<string> Movies)
        {
            List<string> SortedMovies = new List<string>();
            List<MovieAnswer> answers = new List<MovieAnswer>();
            string Movie1 = "";
            string Movie2 = "";


            while (Movies.Count >= 2)
            {
                Movie1 = Movies[0];

                for (int i = 1; i < Movies.Count; i++)
                {
                    Movie2 = Movies[i];

                    var Answer = answers.FirstOrDefault(a => a.Movie1 == Movie1 && a.Movie2 == Movie2);

                    int UserAnswer = 0;

                    if (Answer == null)
                    {
                        //                        Console.Write($"{Movie1}(1) or {Movie2}(2) : ");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(Movie1);

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("(");

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("1");

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(") or ");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(Movie2);

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("(");

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("2");

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(") : ");


                        while (true)
                        {
                            try
                            {
                                Console.ForegroundColor = ConsoleColor.DarkCyan;
                                var userAnswer = Console.ReadLine();

                                if (userAnswer.ToLower() == "ok")
                                {
                                    SortedMovies.Add(Movie1);
                                    Movies.Remove(Movie1);


                                    Movie1 = Movies[0];
                                    i = 0;

                                    break;
                                }
                                UserAnswer = Convert.ToInt32(userAnswer);

                                if (UserAnswer == 1 || UserAnswer == 2)
                                {
                                    break;
                                }
                            }
                            catch (Exception)
                            {

                            }
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("plz enter (1) or (2) : ");
                            Console.ResetColor();
                        }

                        Console.ResetColor();

                        if (UserAnswer != 0)
                        {
                            FixAnswers(Movie1, Movie2, UserAnswer);
                        }

                    }
                    else
                    {
                        UserAnswer = Answer.Answer;
                    }

                    if (UserAnswer == 2)
                    {
                        var ch = Movie1;
                        Movie1 = Movie2;
                        Movie2 = ch;
                    }

                }

                SortedMovies.Add(Movie1);
                Movies.Remove(Movie1);
            }
            if (!SortedMovies.Contains(Movie2))
            {
                SortedMovies.Add(Movie2);
            }


            return SortedMovies;

            void FixAnswers(string movie1, string movie2, int userAnswer)
            {
                #region Current Answer
                answers.Add(new MovieAnswer()
                {
                    Movie1 = movie1,
                    Movie2 = movie2,
                    Answer = userAnswer
                });
                #endregion
                
                //#region Fix Movie 1 and 2
                //if (userAnswer == 2)
                //{
                //    var x = movie1;
                //    movie1 = movie2;
                //    movie2 = x;
                //}
                //#endregion

                //#region Get Second Movies Answers
                //List<string> Movie2Answers = new List<string>();

                //IEnumerable<MovieAnswer> MovieAnswersForMovie2 = answers
                //    .Where(m => (m.Movie1 == movie2 && m.Answer == 1) || (m.Movie2 == movie2 && m.Answer == 2));


                //foreach (var item in MovieAnswersForMovie2)
                //{
                //    string newAnswer = "";

                //    if (item.Answer == 1)
                //    {
                //        newAnswer = item.Movie2;
                //    }
                //    else
                //    {
                //        newAnswer = item.Movie1;
                //    }

                //    if (!Movie2Answers.Contains(newAnswer))
                //    {
                //        Movie2Answers.Add(newAnswer);
                //    }
                //}


                //#endregion

                //#region Add Answers
                //foreach (var item in Movie2Answers)
                //{
                //    var itemIDX = Movies.IndexOf(item);
                //    var SelectedIDX = Movies.IndexOf(movie1);
                //    MovieAnswer newAnswer;
                //    if (SelectedIDX < itemIDX)
                //    {
                //        newAnswer = new MovieAnswer()
                //        {
                //            Answer = 1,
                //            Movie1 = movie1,
                //            Movie2 = item
                //        };
                //    }
                //    else
                //    {
                //        newAnswer = new MovieAnswer()
                //        {
                //            Answer = 2,
                //            Movie1 = item,
                //            Movie2 = movie1
                //        };
                //    }

                //    if (!answers.Any(a => a.Movie1 == newAnswer.Movie1 && a.Movie2 == newAnswer.Movie2))
                //    {
                //        answers.Add(newAnswer);
                //    }
                //}
                //#endregion
            }
        }

        private class MovieAnswer
        {
            public string Movie1 { get; set; }
            public string Movie2 { get; set; }
            public int Answer { get; set; }
        }

        public static void ShowResult(IEnumerable<string> Movies)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("------------------------------------\n");

            for (int i = 1; i <= Movies.Count(); i++)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(i.ToString("00"));

                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(" : ");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(Movies.ElementAt(i - 1));
            }

            Console.ResetColor();
        }
    }
}
