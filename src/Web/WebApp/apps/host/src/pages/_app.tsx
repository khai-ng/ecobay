import { AppProps } from 'next/app';
import Head from 'next/head';
import './styles.css';
import '@base/fonts/fontawesome/css/all.css';
import '@base/fonts/garet/css/style.css';
import { AuthProvider } from '@base/components/auth-context';
import Header from '../components/header';
import { Inter } from 'next/font/google';

const inter = Inter({
  subsets: ['latin'],
  variable: '--font-inter', // Optional: if you're using CSS variables
  display: 'swap', // Optional: controls font display behavior
});

function CustomApp({ Component, pageProps }: AppProps) {
  return (
    <>
      <Head>
        <title>ecobay</title>
      </Head>
      <AuthProvider>
        <main className={inter.className}>
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
