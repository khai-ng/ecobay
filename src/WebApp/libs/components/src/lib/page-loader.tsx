interface PageLoaderProps {
  label: string;
}

const PageLoader = ({ label }: PageLoaderProps) => {
    return <span>{label}</span>;
};

export default PageLoader;