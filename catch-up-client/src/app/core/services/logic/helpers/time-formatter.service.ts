import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TimeFormatterService {

  constructor() { }

  public getTimeAgo(date: Date | string): string {
    const actionDate = typeof date === 'string' ? new Date(date) : date;
    const now = new Date();
    const diffInMs= now.getTime() - actionDate.getTime();
    const diffInMinutes = Math.floor(diffInMs / 60000);

    if(diffInMinutes < 1) {
      return 'just now';
    }
    if(diffInMinutes < 60) {
      return `${diffInMinutes} minute${diffInMinutes === 1 ? '' : 's'} ago`;
    }

    const diffInHours = Math.floor(diffInMinutes / 60);
    if (diffInHours < 24) {
      return `${diffInHours} hour${diffInHours === 1 ? '' : 's'} ago`;
    }

    const diffInDays = Math.floor(diffInHours / 24);
    if (diffInDays < 7) {
      return `${diffInDays} day${diffInDays === 1 ? '' : 's'} ago`;
    }

    const diffInWeeks = Math.floor(diffInDays / 7);
    if (diffInWeeks < 4) {
      return `${diffInWeeks} week${diffInWeeks === 1 ? '' : 's'} ago`;
    }

    const diffInMonths = Math.floor(diffInDays / 30);
    if (diffInMonths < 12) {
      return `${diffInMonths} month${diffInMonths === 1 ? '' : 's'} ago`;
    }

    const diffInYears = Math.floor(diffInMonths / 12);
    return `${diffInYears} year${diffInYears === 1 ? '' : 's'} ago`;
  }
}
