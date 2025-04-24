import Image from 'next/image';

export type DiscoverItemProps = {
  id: string;
  image: string;
  alt: string;
  title: string;
  name: string;
  description: string;
  textClassName?: string;
  onClick?: () => void;
};

const DiscoverItem = (props: DiscoverItemProps) => {
  return (
    <>
      <div className="relative rounded-md">
        <Image
          src={props.image}
          alt={props.alt}
          width={780}
          height={440}
          style={{ borderRadius: 'inherit' }}
        />
        <div
          className={`${props.textClassName} absolute flex flex-col items-start p-4 top-0 gap-2 left-0 h-full`}>
          <span className="text-xs font-medium text-amber-600">
            {props.title}
          </span>
          <span className="text-xl font-bold">{props.name}</span>
          <span className="text-xs text-gray-500">{props.description}</span>
          <button className="flex gap-2 items-center py-2 px-4 rounded-3xl text-xs border border-gray-300 bg-white font-medium">
            <span>Shop now</span>
            <i className="fa-regular fa-arrow-right"></i>
          </button>
        </div>
      </div>
    </>
  );
};

export default DiscoverItem;
