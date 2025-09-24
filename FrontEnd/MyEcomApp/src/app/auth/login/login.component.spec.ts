import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Login } from './login';
import { screen, render, fireEvent } from '@testing-library/angular';

describe('LoginComponent', () => {
  let fixture: ComponentFixture<Login>;
  let component: Login;
  let httpMock: HttpTestingController;
  let router: Router;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule,
        RouterTestingModule,
        FormsModule,
        Login
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(Login);
    component = fixture.componentInstance;
    httpMock = TestBed.inject(HttpTestingController);
    router = TestBed.inject(Router);
    fixture.detectChanges();
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should render login form with username and password inputs', async () => {
    await render(Login, {
      imports: [HttpClientTestingModule, RouterTestingModule, FormsModule, Login]
    });

    expect(screen.getByLabelText(/اسم المستخدم/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/كلمة المرور/i)).toBeInTheDocument();
    expect(screen.getByRole('button', { name: /تسجيل الدخول/i })).toBeInTheDocument();
  });

  it('should call login() and navigate on successful login', async () => {
    await render(Login, {
      imports: [HttpClientTestingModule, RouterTestingModule, FormsModule, Login]
    });

    jest.spyOn(router, 'navigate');

    fireEvent.input(screen.getByLabelText(/اسم المستخدم/i), { target: { value: 'testuser' } });
    fireEvent.input(screen.getByLabelText(/كلمة المرور/i), { target: { value: '123456' } });
    fireEvent.click(screen.getByRole('button', { name: /تسجيل الدخول/i }));

    const req = httpMock.expectOne('https://localhost:5001/api/auth/login');
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual({ userName: 'testuser', password: '123456' });
    req.flush({ accessToken: 'fake-token', refreshToken: 'fake-refresh-token' });

    expect(router.navigate).toHaveBeenCalledWith(['/products']);
  });

  it('should display error message on failed login', async () => {
    await render(Login, {
      imports: [HttpClientTestingModule, RouterTestingModule, FormsModule, Login]
    });

    fireEvent.input(screen.getByLabelText(/اسم المستخدم/i), { target: { value: 'testuser' } });
    fireEvent.input(screen.getByLabelText(/كلمة المرور/i), { target: { value: 'wrongpassword' } });
    fireEvent.click(screen.getByRole('button', { name: /تسجيل الدخول/i }));

    const req = httpMock.expectOne('https://localhost:5001/api/auth/login');
    req.error(new ErrorEvent('Invalid credentials'));

    await fixture.whenStable();
    expect(screen.getByText(/بيانات الدخول غير صحيحة/i)).toBeInTheDocument();
  });
});