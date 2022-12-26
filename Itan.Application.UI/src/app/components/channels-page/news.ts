import {NewsContent} from "../landing-page/news.content";
import {NewsViewModel} from "../../../server/Itan/Core/GetNewsByChannel/NewsViewModel";

export class News {
    constructor(vm: NewsViewModel) {
        this.viewModel = vm;
    }

    viewModel: NewsViewModel;
    content: NewsContent;
    contentVisible: boolean;
    loading: boolean;
}
