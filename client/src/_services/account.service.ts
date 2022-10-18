import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { map, Observable, ReplaySubject } from 'rxjs';
import { User } from '../_models/user';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();
  user: User = {
    username: '',
    token: '',
  };
  constructor(private http: HttpClient, private route: Router) {}
  baseUrl = environment.baseApiUrl;

  login(data: any) {
    return this.http.post<User>(this.baseUrl + 'api/acount/login', data).pipe(
      map((resp: User) => {
        if (resp) {
          localStorage.setItem('user', JSON.stringify(resp));
          this.currentUserSource.next(resp);
        }
      })
    );
  }

  register(data: any) {
    return this.http
      .post<User>(this.baseUrl + 'api/acount/register', data)
      .pipe(
        map((resp: User) => {
          if (resp) {
            localStorage.setItem('user', JSON.stringify(resp));
            this.currentUserSource.next(resp);
          }
        })
      );
  }

  setCurrentUser(user: User) {
    this.currentUserSource.next(user);
  }

  logout() {
    localStorage.removeItem('user');
    return this.currentUserSource.next(null!);
  }
  username() {
    this.currentUserSource.subscribe((resp) => {
      this.user = resp;
    });
    return this.user.username;
  }
}
