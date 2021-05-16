import {Component, Input, OnInit} from '@angular/core';
import {LandingPageNews} from "../landing-page/landing-page.news";

@Component({
  selector: 'app-landing-page-news',
  templateUrl: './landing-page-news.component.html',
  styleUrls: ['./landing-page-news.component.scss']
})
export class LandingPageNewsComponent implements OnInit {

  @Input() news:LandingPageNews;

  constructor() { }

  ngOnInit(): void {
  }

  hasImage(): boolean {
    return this.news?.content?.Image?.length > 0;
  }

  getImage(): string {
    if (this.news?.content?.Image.startsWith('https')) {
      return this.news.content?.Image;
    }
    return `https://itan-app-service-function.azurewebsites.net/api/HttpHttpsImage?url=${this.news.content?.Image}`;
  }

  display(): string {
    return this.news.content?.Description ?? this.news.content?.Content;
  }

  onExternalLinkClick() {
    window.open(this.news.viewModel.link);
  }

}
