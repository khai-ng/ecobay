import React from "react";
import Image from "next/image";
const Header = () => {
    return (
      <div className="app_container text-sm md:text-base flex justify-center items-center gap-3 py-3">
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
        <div className="flex gap-1 items-center p-2">
          <i className="fa-regular fa-cart-shopping"></i>
          <span className="hidden sm:block">Cart</span>
        </div>
        <div className="flex gap-1 items-center p-2">
          <i className="fa-duotone fa-solid fa-user"></i>
          <span className="hidden sm:block">Sign in</span>
        </div>
      </div>
    );
};

export default Header;