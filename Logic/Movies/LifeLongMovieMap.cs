using FluentNHibernate.Mapping;

namespace Logic.Movies
{
    public class LifeLongMovieMap : SubclassMap<LifeLongMovie>
    {
        public LifeLongMovieMap()
        {
            DiscriminatorValue(2);
        }
    }
}