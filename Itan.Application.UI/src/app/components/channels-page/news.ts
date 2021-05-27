import {NewsViewModel} from "../../../server/Itan/Core/NewsViewModel";
import {NewsContent} from "../landing-page/news.content";

export class News {
    constructor(vm: NewsViewModel) {
        this.viewModel = vm;
    }

    viewModel: NewsViewModel;
    content: NewsContent;
    contentVisible: boolean;
    loading: boolean;
}
