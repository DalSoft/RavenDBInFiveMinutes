using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client;
using RavenDBInFiveMinutes.Website.Models;

namespace RavenDBInFiveMinutes.Tests.Unit
{
    internal static class TestExtensions
    {
        public static Movie SaveNewMovieToRavenDB(this IDocumentSession documentSession, Movie movie)
        {
            
            documentSession.Store(movie);
            documentSession.SaveChanges();
            return movie;
        }
        
        //Minor extension abuse :)
        public static Movie NewValid(this Movie movie) 
        {
            return new Movie { Description = "Test-Description" + Guid.NewGuid(), Title = "Test-Title" + Guid.NewGuid() }; 
        }

        public static Movie NewInvalid(this Movie movie)
        {
            return new Movie { Description = null, Title = null };
        }
        //
    }
}
