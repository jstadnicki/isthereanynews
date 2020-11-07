import {Component, OnInit} from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";

@Component({
  selector: 'app-landing-page',
  templateUrl: './landing-page.component.html',
  styleUrls: ['./landing-page.component.less']
})
export class LandingPageComponent implements OnInit {

  constructor(
    private http: HttpClient) {
  }

  ngOnInit(): void {
    this.loadLandingPageNews();
    setInterval(() => {
      this.topIndex = this.topIndex < this.news.topNews.length - 1 ? this.topIndex + 1 : 0;
      this.bottomPage = this.bottomPage * 3 <= this.news.bottomNews.length - 1 ? this.bottomPage + 3 : 0;
    }, 5000);
  }

  news: HomePageNews;
  topIndex: number = 0;
  bottomPage: number = 0;
  loaded: boolean = false;

  loadLandingPageNews() {
    this.http.get<HomePageNews>('https://itan-app-service-webapi.azurewebsites.net/api/landingpage/news')
      .subscribe(result => {
        this.news = result;
        this.loadContent();
      })
  }

  hasImage(news: LandingPageNews): boolean {
    return news?.content?.Image?.length > 0;
  }

  private loadContent() {
    this.news.topNews.forEach((newsItem) => {
      const url = newsItem.contentLink;
      let headers = new HttpHeaders();
      headers.append("Origin", "http://localhost:4200");
      let options = {headers: headers}
      this.http
        .get<NewsContent>(url, options)
        .subscribe(response => {
          newsItem.content = response;

          var tempDiv = document.createElement('div');
          tempDiv.innerHTML = this.display(newsItem);
          var firstImage = tempDiv.getElementsByTagName('img')[0]
          var imgSrc = firstImage ? firstImage.src : "";

          newsItem.content.Image = imgSrc;
        });
    });

    this.news.bottomNews.forEach((newsItem) => {
      const url = newsItem.contentLink;
      let headers = new HttpHeaders();
      headers.append("Origin", "http://localhost:4200");
      let options = {headers: headers}
      this.http
        .get<NewsContent>(url, options)
        .subscribe(response => {
          newsItem.content = response;

          var tempDiv = document.createElement('div');
          tempDiv.innerHTML = this.display(newsItem);
          var firstImage = tempDiv.getElementsByTagName('img')[0]
          var imgSrc = firstImage ? firstImage.src : "";

          newsItem.content.Image = imgSrc;
        });
    });
    this.loaded = true;
  }

  display(news: LandingPageNews): string {
    return news.content?.Description ?? news.content?.Content;
  }

  onExternalLinkClick(newsItan: LandingPageNews) {
    window.open(newsItan.link);
  }
}

class HomePageNews {
  topNews: LandingPageNews[];
  bottomNews: LandingPageNews[];
}

class LandingPageNews {
  constructor() {
    this.content = new NewsContent();
  }

  author: string;
  title: string;
  id: string;
  published: Date
  link: string;
  image: string;
  content: NewsContent;
  contentLink: string;
}

class NewsContent {
  Content: string;
  Description: string;
  Image: string;
  Author: string;
  Link: string;
}

