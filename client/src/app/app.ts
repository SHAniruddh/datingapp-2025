import { HttpClient } from '@angular/common/http';
import { Component, OnInit, inject, signal } from '@angular/core';
import { lastValueFrom } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrls: ['./app.css']
})
export class App implements OnInit {

  private http = inject(HttpClient);
  protected title = 'Dating App';
  protected members = signal<any[]>([]); // signal for reactive state

  async ngOnInit() {
    try {
      const response = await lastValueFrom(
        this.http.get<any[]>('http://localhost:5180/api/members')
      );
      this.members.set(response);
    } catch (error) {
      console.error('Failed to fetch members:', error);
    }
  }

  // Used for trackBy in *ngFor
  trackById(index: number, member: any): number {
    return member.id;
  }
}
