import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { AppComponent } from './app.component';

describe('AppComponent', () => {
  let component: AppComponent;
  let fixture: ComponentFixture<AppComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      declarations: [AppComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(AppComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the app', () => {
    expect(component).toBeTruthy();
  });

  it(`should have as title 'stb-app'`, () => {
    expect(component.title).toEqual('stb-app');
  });

  it('should initialize userName with "UTILISATEUR" if no login is stored', () => {
    localStorage.removeItem('login');
    component.updateUserName();
    expect(component.userName).toBe('UTILISATEUR');
  });

  it('should set userName from localStorage if login exists', () => {
    localStorage.setItem('login', 'TestUser');
    component.updateUserName();
    expect(component.userName).toBe('TestUser');
  });

  it('should remove token and login on logout', () => {
    localStorage.setItem('token', '123');
    localStorage.setItem('login', 'TestUser');
    component.logout();
    expect(localStorage.getItem('token')).toBeNull();
    expect(localStorage.getItem('login')).toBeNull();
    expect(component.userName).toBe('');
  });
});
