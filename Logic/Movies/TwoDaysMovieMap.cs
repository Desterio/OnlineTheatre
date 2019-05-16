using FluentNHibernate.Mapping;

namespace Logic.Movies
{
    public class TwoDaysMovieMap : SubclassMap<TwoDaysMovie>
    {
        public TwoDaysMovieMap()
        {
            DiscriminatorValue(1);
        }
    }
}