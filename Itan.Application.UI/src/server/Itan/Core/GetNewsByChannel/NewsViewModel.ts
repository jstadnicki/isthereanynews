import { NewsHeaderTagViewModel } from './NewsHeaderTagViewModel';

export class NewsViewModel
{
	public id: any;
	public title: string;
	public contentUrl: string;
	public published: any;
	public link: string;
	public read: boolean;
	public originalPostId: any;
	public tags: NewsHeaderTagViewModel[];
}
