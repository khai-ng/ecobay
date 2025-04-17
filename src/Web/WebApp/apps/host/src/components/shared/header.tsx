import React from "react";
import Image from "next/image";
import { useAuth } from "@shared/components/auth-context";
import { useRouter } from "next/router";

const Header = () => {
  const { keycloak, isAuthenticated } = useAuth();
  const router = useRouter();
  
  const handleSignIn  = () => {
      keycloak?.login();

  };

  const handleSignOut = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('refreshToken');
    keycloak?.logout();
  };

  const handleCartRedirection = () => {
      router.push('/cart');
  }

  return (
    <header className="text-sm md:text-base flex justify-center items-center gap-3 py-3 hidden">
      <div>
        <Image
          src={'/images/ecobay.svg'}
          alt="ecobay"
          width={157}
          height={55} 
        />
      </div>
      <div className="flex gap-1 items-center grow rounded-md border-2 border-gray-400">
        <input
          className="grow p-1 focus:outline-none rounded-[inherit]"
          type="text"
          placeholder="Search on ecobay..."
        />
        <i className="fa-regular fa-magnifying-glass p-3"></i>
      </div>
      <div className="flex gap-1 items-center p-2" onClick={handleCartRedirection}>
        <i className="fa-regular fa-cart-shopping"></i>
        <span className="hidden sm:block">Cart</span>
      </div>
      <div className="flex gap-1 items-center p-2" onClick={isAuthenticated ? handleSignOut : handleSignIn}>
        <i className="fa-duotone fa-solid fa-user"></i>
        <span className="hidden sm:block">{isAuthenticated ? keycloak?.tokenParsed?.name : 'Sign In'}</span>
      </div>
    </header>
  );
};

export default Header;