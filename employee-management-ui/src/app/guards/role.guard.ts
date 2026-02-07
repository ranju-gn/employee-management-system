import { inject } from '@angular/core';
import { Router, CanActivateFn } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { ToastrService } from 'ngx-toastr';

export const roleGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const toastr = inject(ToastrService);

  const requiredRoles = route.data['roles'] as string[];
  const userRole = authService.getUserRole();

  if (requiredRoles && !requiredRoles.includes(userRole)) {
    toastr.error(
      'You do not have permission to access this page',
      'Access Denied',
    );
    router.navigate(['/dashboard']);
    return false;
  }

  return true;
};
