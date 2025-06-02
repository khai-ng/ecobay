import Image from 'next/image';
import Banner from '../components/banner';
import ProductListLoader from '../components/product-list.loader';
import DiscoverItem, { DiscoverItemProps } from './components/discover-item';
import { GetProductRequest } from '../lib/product/product.model';

export function Index() {
  const getArrivalRequest: GetProductRequest = {
    pageIndex: 1,
    pageSize: 6,
  };
  const getBestSellerRequest: GetProductRequest = {
    pageIndex: 1,
    pageSize: 10,
  };

  const discover1Items: DiscoverItemProps[] = [
    {
      id: '1',
      image: '/images/discover/banner-01.jpg',
      alt: 'discover 1',
      title: 'Only This Week',
      name: 'Quality eggs at an affordable price',
      description: 'Eat one every day',
      textClassName: 'w-2/3 justify-center',
    },
    {
      id: '1',
      image: '/images/discover/banner-02.jpg',
      alt: 'discover 1',
      title: 'Only This Week',
      name: 'Snacks that nourishes our mind and body',
      description: 'Shine the morning...',
      textClassName: 'w-2/3 justify-center',
    },
    {
      id: '1',
      image: '/images/discover/banner-03.jpg',
      alt: 'discover 1',
      title: 'Only This Week',
      name: 'Unbeatable quality, unbeatable prices.',
      description: 'Only this week. Don’t miss...',
      textClassName: 'w-2/3 justify-center',
    },
  ];

  const discover2Items: DiscoverItemProps[] = [
    {
      id: '1',
      image: '/images/discover/banner-05.jpg',
      alt: 'discover 1',
      title: 'Only This Week',
      name: 'Provides you experienced quality products',
      description: 'Feed your family the best',
    },
    {
      id: '1',
      image: '/images/discover/banner-05.jpg',
      alt: 'discover 1',
      title: 'Only This Week',
      name: 'Shopping with us for better quality and the best price',
      description: 'Only this week. Don’t miss...',
    },
    {
      id: '1',
      image: '/images/discover/banner-06.jpg',
      alt: 'discover 1',
      title: 'Only This Week',
      name: 'Get the best quality products at the lowest prices',
      description: 'A different kind of grocery store',
    },
    {
      id: '1',
      image: '/images/discover/banner-07.jpg',
      alt: 'discover 1',
      title: 'Only This Week',
      name: 'Where you get your all favorite brands under one roof',
      description: 'Only this week. Don’t miss...',
    },
  ];
  
  return (
    <>
      <Banner />
      <br />
      <div className="flex justify-center">
        <div className="flex h-[80px] gap-4">
          <Image
            src={`/images/headlines/online-pay.svg`}
            width={50}
            height={50}
            alt="online payment"
            className="self-end"
          />
          <div className="flex flex-col justify-center self-start">
            <span className="font-bold">Payment only online</span>
            <span className="text-gray-400 text-xs">
              Tasigförsamhet beteendedesign. Mobile checkout. Ylig kärrtorpa.
            </span>
          </div>
        </div>
        <div className="flex h-[80px] gap-4">
          <Image
            src={`/images/headlines/discount.svg`}
            width={50}
            height={50}
            alt="online payment"
            className="self-end"
          />
          <div className="flex flex-col justify-center self-start">
            <span className="font-bold">New stocks and sales</span>
            <span className="text-gray-400 text-xs">
              Tasigförsamhet beteendedesign. Mobile checkout. Ylig kärrtorpa.
            </span>
          </div>
        </div>
        <div className="flex h-[80px] gap-4">
          <Image
            src={`/images/headlines/quality.svg`}
            width={50}
            height={50}
            alt="online payment"
            className="self-end"
          />
          <div className="flex flex-col justify-center self-start">
            <span className="font-bold">Quality assurance</span>
            <span className="text-gray-400 text-xs">
              Tasigförsamhet beteendedesign. Mobile checkout. Ylig kärrtorpa.
            </span>
          </div>
        </div>
        <div className="flex h-[80px] gap-4">
          <Image
            src={`/images/headlines/delivery.svg`}
            width={50}
            height={50}
            alt="online payment"
            className="self-end"
          />
          <div className="flex flex-col justify-center self-start">
            <span className="font-bold">Delivery from 1 hour</span>
            <span className="text-gray-400 text-xs">
              Tasigförsamhet beteendedesign. Mobile checkout. Ylig kärrtorpa.
            </span>
          </div>
        </div>
      </div>

      <div className="h-[1px] bg-gray-300 mt-4 mb-6"></div>

      <div className="flex justify-center gap-4">
        {discover1Items.map((item, index) => (
          <DiscoverItem key={index} {...item} />
        ))}
      </div>

      <div>
        <div className="flex items-end py-6 gap-4">
          <span className="font-bold text- leading-none">New Arrivals</span>
          <span className="text-xs leading-none text-gray-400">
            Don&apos;t miss this opportunity at a special discount just for this
            week.
          </span>
        </div>
        <ProductListLoader className='grid grid-cols-6 border border-gray-300 rounded-md' request={getArrivalRequest} />
      </div>

      <div className="flex justify-center gap-4 mt-4">
        {discover2Items.map((item, index) => (
          <DiscoverItem key={index} {...item} />
        ))}
      </div>

      <div>
        <div className="flex items-end py-6 gap-4">
          <span className="font-bold text- leading-none">Best Sellers</span>
          <span className="text-xs leading-none text-gray-400">
            Don&apos;t miss this opportunity at a special discount just for this
            week.
          </span>
        </div>
        <ProductListLoader className='grid grid-cols-5 border border-gray-300 rounded-md' request={getBestSellerRequest} />
      </div>
    </>
  );
}

export default Index;
