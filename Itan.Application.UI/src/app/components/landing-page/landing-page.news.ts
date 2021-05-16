import {LandingPageNewsViewModel} from "../../../server/Itan/Core/LandingPageNewsViewModel";
import {NewsContent} from "./news.content";

export class LandingPageNews {
    constructor(vm: LandingPageNewsViewModel) {
        this.content = new NewsContent();
        this.viewModel = vm;
    }

    viewModel: LandingPageNewsViewModel;
    content: NewsContent;
}
