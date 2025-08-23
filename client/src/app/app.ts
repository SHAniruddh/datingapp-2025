import { Component, OnInit, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common'; // ✅ Import this
import { lastValueFrom } from 'rxjs';

@Component({
  selector: 'app-root',
  standalone: true, // ✅ must be true for standalone component
  imports: [CommonModule], // ✅ add this line
  templateUrl: './app.html',
  styleUrls: ['./app.css']
})
export class App implements OnInit {

  private http = inject(HttpClient);
  protected title = 'Dating App';
  protected members = signal<any[]>([]);

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

  trackById(index: number, member: any): number {
    return member.id;
  }
}
