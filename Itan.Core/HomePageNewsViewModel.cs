using System.Collections.Generic;

namespace Itan.Core
{
    public class HomePageNewsViewModel
    {
        public List<LandingPageNewsViewModel> TopNews { get; set; } = new List<LandingPageNewsViewModel>();
        public List<LandingPageNewsViewModel> BottomNews { get; set; } = new List<LandingPageNewsViewModel>();
    }
}