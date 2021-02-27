using System;
using MovieRating;
using System.Collections.Generic;
using System.Linq;




var Movies = Movie.GetMovies();

Movies = Movies.SortMovies();

Movie.ShowResult(Movies);


Console.ReadKey();