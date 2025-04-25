import React from 'react';
import Image from 'next/image';
import { useAuth } from '@base/components/auth-context';
import Link from 'next/link';

const Header = () => {
  const { keycloak, isAuthenticated } = useAuth();

  const handleSignIn = () => {
    keycloak?.login();
  };

  const handleSignOut = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('refreshToken');
    keycloak?.logout();
  };

  return (
    <header className="text-sm md:text-base flex flex-col justify-center items-center gap-3 mb-3">
      <div className="flex gap-3 w-full justify-between">
        <div className="flex gap-3">
          <span className="text-gray-500 text-sm">About us</span>
          <span className="text-gray-500 text-sm">My account</span>
          <span className="text-gray-500 text-sm">Wishlist</span>
        </div>
        <div className="flex gap-3">
          <span className="text-gray-500 text-sm">English</span>
          <span className="text-gray-500 text-sm">USD</span>
          <span className="text-gray-500 text-sm">Order tracking</span>
        </div>
      </div>

      <div className="flex gap-6 w-full justify-between">
        <div className="relative flex justify-center items-center gap-1">
          <Image src={'/images/logo.svg'} alt="ecobay" width={45} height={45} />
          <span className="text-2xl font-bold">ecobay</span>
          <span className="absolute top-0 right-0 text-[8px] text-primary font-bold leading-4">
            com
          </span>
        </div>

        <div className="flex justify-center items-center gap-3">
          <i className="fa-light fa-location-dot text-2xl"></i>
          <div className="flex flex-col">
            <span className="text-xs text-gray-500">Deliver to</span>
            <span className="leading-none">all</span>
          </div>
        </div>
        <div className="flex gap-1 items-center grow rounded-md bg-secondary">
          <input
            type="text"
            className="grow p-2 focus:outline-none rounded-[inherit] bg-secondary text-gray-700 text-sm font-light"
            placeholder="Search for products, categories or brands..."
          />
          <i className="fa-regular fa-magnifying-glass p-3"></i>
        </div>
        <div className="flex gap-1">
          <Link href="/cart" className="flex gap-1 items-center p-2">
            <i className="fa-regular fa-cart-shopping"></i>
          </Link>
          <div className="flex gap-1 items-center p-2">
            <i className="fa-regular fa-heart"></i>
          </div>
          <div
            className="flex gap-1 items-center p-2"
            onClick={isAuthenticated ? handleSignOut : handleSignIn}>
            <i className="fa-regular fa-user"></i>
            <span className="hidden sm:block">
              {isAuthenticated ? keycloak?.tokenParsed?.name : 'Sign In'}
            </span>
          </div>
        </div>
      </div>
    </header>
  );
};

export default Header;
