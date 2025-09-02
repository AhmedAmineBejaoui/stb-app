import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd, Event } from '@angular/router';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit { 
  title = 'stb-app';
  showHeaderSidebar = true;
  isAuthRoute = false;
  userName: string = '';

  constructor(private router: Router) {
    this.router.events
      .pipe(filter((event: Event): event is NavigationEnd => event instanceof NavigationEnd))
      .subscribe((event: NavigationEnd) => {
        this.isAuthRoute = event.urlAfterRedirects.startsWith('/auth');
        this.showHeaderSidebar = !this.isAuthRoute;
        this.updateUserName(); 
      });
  }

  ngOnInit() {
    this.updateUserName(); 
  }

  updateUserName() {
    const login = localStorage.getItem("login");
    this.userName = login ? login : 'UTILISATEUR';
    console.log('Username:', this.userName); 
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('login');
    this.userName = '';
    this.router.navigate(['/auth']);
  }
}
