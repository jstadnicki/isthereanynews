import {Component, OnInit} from "@angular/core";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {HomePageNewsViewModel} from '../../../server/Itan/Core/HomePageNewsViewModel'
import {LandingPageNews} from "./landing-page.news";
import {HomePageNews} from "./home-page.news";
import {NewsContent} from "./news.content";

@Component({
  selector: 'app-landing-page',
  templateUrl: './landing-page.component.html',
  styleUrls: ['./landing-page.component.less']
})
export class LandingPageComponent implements OnInit {
  private newsCarouselTimer: number;

  constructor(
    private http: HttpClient) {
  }

  ngOnInit(): void {
    this.loadLandingPageNews();
  }

  startNewsCarousel(){
    this.newsCarouselTimer = setInterval(() => {
      this.topIndex = this.topIndex < this.news.topNews.length - 1 ? this.topIndex + 1 : 0;
      this.bottomPage = this.bottomPage * 3 <= this.news.bottomNews.length - 1 ? this.bottomPage + 3 : 0;
    }, 5000);
  }

  ngOnDestroy():void{
    clearInterval(this.newsCarouselTimer);
  }

  news: HomePageNews;
  topIndex: number = 0;
  bottomPage: number = 0;
  loaded: boolean = false;

  loadLandingPageNews() {
    this.http.get<HomePageNewsViewModel>(`${environment.apiUrl}/api/landingpage/news`)
      .subscribe(result => {
        this.news = new HomePageNews(result);
        this.loadContent();
        this.startNewsCarousel();
      })
  }

  hasImage(news: LandingPageNews): boolean {
    return news?.content?.Image?.length > 0;
  }

  getImage(news: LandingPageNews): string {
    if (news?.content?.Image.startsWith('https')) {
      return news.content?.Image;
    }
    return `https://itan-app-service-function.azurewebsites.net/api/HttpHttpsImage?url=${news.content?.Image}`;
  }

  private loadContent() {
    this.news.topNews.forEach((newsItem) => {
      const url = newsItem.viewModel.contentLink;
      let headers = new HttpHeaders();
      let options = {headers: headers}
      this.http
        .get<NewsContent>(url, options)
        .subscribe(response => {
          newsItem.content = response;

          const tempDiv = document.createElement('div');
          tempDiv.innerHTML = this.display(newsItem);
          const firstImage = tempDiv.getElementsByTagName('img')[0];
          const imgSrc = firstImage ? firstImage.src : "";

          newsItem.content.Image = imgSrc;
        }, error => {
          console.log(error);
        });
    });

    this.news.bottomNews.forEach(newsItem => {
      const url = newsItem.viewModel.contentLink;
      let headers = new HttpHeaders();
      let options = {headers: headers}
      this.http
        .get<NewsContent>(url, options)
        .subscribe(response => {
          newsItem.content = response;

          const tempDiv = document.createElement('div');
          tempDiv.innerHTML = this.display(newsItem);
          const firstImage = tempDiv.getElementsByTagName('img')[0];
          const imgSrc = firstImage ? firstImage.src : "";

          newsItem.content.Image = imgSrc;
        }, error => {
          console.log(error);
        });
    });
    this.loaded = true;
  }

  display(news: LandingPageNews): string {
    return news.content?.Description ?? news.content?.Content;
  }

  onExternalLinkClick(newsItan: LandingPageNews) {
    window.open(newsItan.viewModel.link);
  }
}

