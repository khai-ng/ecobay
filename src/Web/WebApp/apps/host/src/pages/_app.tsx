import { AppProps } from 'next/app';
import Head from 'next/head';
import './styles.css';
import '@shared/fonts/fontawesome/css/all.css';
import '@shared/fonts/garet/css/style.css';
import { AuthProvider } from '@shared/components/auth-context';
import Header from '../components/shared/header';

function CustomApp({ Component, pageProps }: AppProps) {
  return (
    <>
      <Head>
        <title>ecobay</title>
      </Head>
      
      <AuthProvider>
        <main className="bg-gray-100">
          <div className="app_container bg-white">
            <Header />
            <Component {...pageProps} />
          </div>
        </main>  
      </AuthProvider>  
    </>
  );
}

export default CustomApp;
