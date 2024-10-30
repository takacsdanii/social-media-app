import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MediaUrlService {
  private baseUrl: string = 'https://localhost:7175';

  constructor() { }

  public getFullUrl(url: string | null | undefined): string | null {
    if(url) return `${this.baseUrl}${url}`;
    else return null;
  }
}
