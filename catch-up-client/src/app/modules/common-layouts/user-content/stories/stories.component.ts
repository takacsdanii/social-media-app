import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-stories',
  templateUrl: './stories.component.html',
  styleUrl: './stories.component.scss'
})
export class StoriesComponent implements OnInit {
  @Input() public userId: string;
  public stories: [];
  
  constructor() { }

  public ngOnInit(): void {
      
  }
}
