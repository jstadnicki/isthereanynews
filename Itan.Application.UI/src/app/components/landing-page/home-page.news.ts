import {HomePageNewsViewModel} from "../../../server/Itan/Core/HomePageNewsViewModel";
import {LandingPageNews} from "./landing-page.news";

export class HomePageNews {
    constructor(vm: HomePageNewsViewModel) {
        this.viewModel = vm;
        this.bottomNews = vm.bottomNews.map(n => new LandingPageNews(n));
        this.topNews = vm.topNews.map(n => new LandingPageNews(n));
    }

    private viewModel: HomePageNewsViewModel;
    topNews: LandingPageNews[];
    bottomNews: LandingPageNews[];
}
