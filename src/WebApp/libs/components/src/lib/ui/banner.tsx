import styles from './banner.module.css';

export const Banner = () => {
  return (
    <div
      className={`${styles.banner__container} text-sm md:text-base lg sm:h-[24rem]`}>
      <div className={`${styles.area_cate} border border-gray-300 rounded-lg`}>
        <div className="flex items-center gap-2 border-b border-gray-300 px-4 py-2 cursor-pointer hover:bg-gray-100">
          <i className="fa-regular fa-bars-sort"></i>
          <span>All Categories</span>
        </div>

        <ul>
          <li>
            <i className="fa-regular fa-apple-whole"></i>
            <span>Fruits & Vegetables</span>
          </li>
        </ul>
        <ul>
          <li>
            <i className="fa-light fa-steak"></i>
            <span>Meats & Seafood</span>
          </li>
        </ul>
        <ul>
          <li>
            <i className="fa-regular fa-bowl-food"></i>
            <span>Bakery & Snacks</span>
          </li>
        </ul>
        <ul>
          <li>
            <i className="fa-regular fa-bowl-food"></i>
            <span>Bakery & Snacks</span>
          </li>
        </ul>
        <ul>
          <li>
            <i className="fa-regular fa-bowl-food"></i>
            <span>Bakery & Snacks</span>
          </li>
        </ul>
        <ul>
          <li>
            <i className="fa-regular fa-bowl-food"></i>
            <span>Bakery & Snacks</span>
          </li>
        </ul>
        <ul>
          <li>
            <i className="fa-regular fa-bowl-food"></i>
            <span>Bakery & Snacks</span>
          </li>
        </ul>
        <ul>
          <li>
            <i className="fa-regular fa-bowl-food"></i>
            <span>Bakery & Snacks</span>
          </li>
        </ul>
      </div>

      <div className={`${styles.area_slide} overflow-hidden`}>
        <div className="flex h-full">
          <div className="shrink-0 w-full">
            <img src='/images/banner1.jpg' alt='banner' width={750} height={350} className='h-full w-full object-cover'></img>
          </div>
          <div className="shrink-0 w-full">
            <img src='/images/banner2.jpg' alt='banner' width={750} height={350} className='h-full w-full object-cover'></img>
          </div>
          <div className="shrink-0 w-full">
            <img src='/images/banner3.jpg' alt='banner' width={750} height={350} className='h-full w-full object-cover'></img>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Banner;
