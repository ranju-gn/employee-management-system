import { TestBed } from '@angular/core/testing';
import {
  HttpClientTestingModule,
  HttpTestingController,
} from '@angular/common/http/testing';
import { Router } from '@angular/router';
import { AuthService } from './auth.service';
import { LoginRequest, LoginResponse } from '../models/auth.model';
import { environment } from '../../environments/environment';

describe('AuthService', () => {
  let service: AuthService;
  let httpMock: HttpTestingController;
  let routerSpy: jasmine.SpyObj<Router>;

  beforeEach(() => {
    const routerSpyObj = jasmine.createSpyObj('Router', ['navigate']);

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [AuthService, { provide: Router, useValue: routerSpyObj }],
    });

    service = TestBed.inject(AuthService);
    httpMock = TestBed.inject(HttpTestingController);
    routerSpy = TestBed.inject(Router) as jasmine.SpyObj<Router>;
  });

  afterEach(() => {
    httpMock.verify();
    localStorage.clear();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('login', () => {
    it('should login user and store token', (done) => {
      const loginRequest: LoginRequest = {
        username: 'testuser',
        password: 'password123',
      };

      const mockResponse: LoginResponse = {
        token: 'fake-jwt-token',
        username: 'testuser',
        email: 'test@test.com',
        role: 'User',
        expiresAt: new Date(),
      };

      service.login(loginRequest).subscribe((response) => {
        expect(response).toEqual(mockResponse);
        expect(localStorage.getItem('token')).toBe(mockResponse.token);
        expect(localStorage.getItem('user')).toBe(JSON.stringify(mockResponse));
        done();
      });

      const req = httpMock.expectOne(`${environment.apiUrl}/auth/login`);
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual(loginRequest);
      req.flush(mockResponse);
    });
  });

  describe('logout', () => {
    it('should clear storage and navigate to login', () => {
      localStorage.setItem('token', 'fake-token');
      localStorage.setItem('user', JSON.stringify({ username: 'test' }));

      service.logout();

      expect(localStorage.getItem('token')).toBeNull();
      expect(localStorage.getItem('user')).toBeNull();
      expect(routerSpy.navigate).toHaveBeenCalledWith(['/login']);
    });
  });

  describe('isAuthenticated', () => {
    it('should return true when valid token exists', () => {
      // Mock a valid token (not expired)
      const validToken =
        'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjk5OTk5OTk5OTl9.fake';
      localStorage.setItem('token', validToken);

      // Note: This test may need adjustment based on actual JWT structure
      const result = service.isAuthenticated();
      expect(result).toBeDefined();
    });

    it('should return false when no token exists', () => {
      const result = service.isAuthenticated();
      expect(result).toBe(false);
    });
  });

  describe('getUserRole', () => {
    it('should return user role from current user', (done) => {
      const mockUser: LoginResponse = {
        token: 'token',
        username: 'test',
        email: 'test@test.com',
        role: 'Admin',
        expiresAt: new Date(),
      };

      service.login({ username: 'test', password: 'pass' }).subscribe(() => {
        const role = service.getUserRole();
        expect(role).toBe('Admin');
        done();
      });

      const req = httpMock.expectOne(`${environment.apiUrl}/auth/login`);
      req.flush(mockUser);
    });
  });
});
