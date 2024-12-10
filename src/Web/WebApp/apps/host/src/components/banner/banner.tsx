import React from "react";
import banner from './banner.module.css';
import Image from 'next/image';

// interface BannerProps {

// }

const Banner = () => {
    return (
      <div className={`${banner.banner__container} app_container text-sm md:text-base lg sm:h-[24rem]`}>
        <ul className={`${banner.area_cate} overflow-auto px-2 my-3 border border-gray-300 rounded-lg`}>
          <li>Electronic</li>
          <li>Computer</li>
          <li>Start Home</li>
          <li>Arts & Crafts</li>
          <li>Automotive</li>
          <li>Baby</li>
          <li>Beauty and Personal Care</li>
          <li>Women&apos;s Fashion</li>
          <li>Men&apos;s Fashion</li>
          <li>Girls&apos; Fashion</li>
          <li>Boys&apos; Fashion</li>
          <li>Health and Household</li>
          <li>Home and Kitchen</li>
          <li>Industrial and Scientific</li>
          <li>Luggage</li>
        </ul>
        <div className={`${banner.area_slide} overflow-hidden`}>
          <div className="flex h-full">
            <div className="shrink-0 w-full">
              <Image
                src={`/images/banner1.png`}
                alt="banner"
                width={780}
                height={440}
                style={{height: "100%", objectFit: "contain"}}
              />
            </div>
            <div className="shrink-0 w-full">
              <Image
                src={`/images/banner2.png`}
                alt="banner"
                width={780}
                height={440}
                style={{height: "100%", objectFit: "contain"}}
              />
            </div>
            <div className="shrink-0 w-full">
              <Image
                src={`/images/banner3.png`}
                alt="banner"
                width={780}
                height={440}
                style={{height: "100%", objectFit: "contain"}}
              />
            </div>
          </div>
        </div>
        <div className={`${banner.area_bottom} flex justify-center gap-3 `}>
          <div className="flex flex-col items-center gap-1 flex-1 p-2 border rounded-md border-gray-400">
            <i className="text-xl fa-regular fa-thumbs-up"></i>            
            <span className="flex-1 text-sm text-center leading-5 line-clamp-2">Top Choice</span>
          </div>
          <div className="flex flex-col items-center gap-1 flex-1 p-2 border rounded-md border-gray-400">
            <i className="text-xl fa-sharp fa-regular fa-badge-percent"></i>
            <span className="flex-1 text-sm text-center leading-5 line-clamp-2">Xả kho giảm giá</span>
          </div>
          <div className="flex flex-col items-center gap-1 flex-1 p-2 border rounded-md border-gray-400">
            <i className="text-xl fa-regular fa-laptop-mobile"></i>
            <span className="flex-1 text-sm text-center leading-5 line-clamp-2">Đồng công nghệ</span>
          </div>
          <div className="flex flex-col items-center gap-1 flex-1 p-2 border rounded-md border-gray-400">
            <i className="text-xl fa-regular fa-baby-carriage"></i>
            <span className="flex-1 text-sm text-center leading-5 line-clamp-2">Mẹ và bé</span>
          </div>
          <div className="flex flex-col items-center gap-1 flex-1 p-2 border rounded-md border-gray-400">
            <i className="text-xl fa-regular fa-kitchen-set"></i>
            <span className="flex-1 text-sm text-center leading-5 line-clamp-2">Đồ dùng nhà bếp</span>
          </div>
        </div>
        <div className={`${banner.area_right} flex flex-col gap-3 my-3`}>
          <Image
            src={`/images/right-banner-mac-mini-m4.webp`}
            alt="banner"
            width={350}
            height={300}
          />
          <Image
            src={`/images/mc-m4-mb.webp`}
            alt="banner"
            width={350}
            height={300}
          />
          <Image
            src={`/images/iphone-16-pro-km-moi.webp`}
            alt="banner"
            width={350}
            height={300}
          />
        </div>
      </div>
    );
}

export default Banner;