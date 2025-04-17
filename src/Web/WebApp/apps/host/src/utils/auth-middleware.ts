import { NextResponse } from 'next/server';
import type { NextRequest } from 'next/server';

export function middleware(request: NextRequest) {
  // Add your authentication check logic here
  const token = request.cookies.get('KEYCLOAK_TOKEN');
  
  if (!token && !request.nextUrl.pathname.startsWith('/auth')) {
    return NextResponse.redirect(new URL('/auth/login', request.url));
  }
  
  return NextResponse.next();
}

export const config = {
  matcher: [
    // Add paths that require authentication
    // '/dashboard/:path*',
    // '/profile/:path*',

    '/cart/:path*'
  ],
};