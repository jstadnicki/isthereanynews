using System.Collections.Generic;

namespace Itan.Core
{
    public class HomePageNews
    {
        public List<LandingPageNews> TopNews { get; set; } = new List<LandingPageNews>();
        public List<LandingPageNews> BottomNews { get; set; } = new List<LandingPageNews>();
    }
}