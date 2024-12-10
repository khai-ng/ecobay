import { AppProps } from 'next/app';
import Head from 'next/head';
import './styles.css';
import '@shared/fonts/fontawesome/css/all.css';
import '@shared/fonts/garet/css/style.css';

function CustomApp({ Component, pageProps }: AppProps) {
  return (
    <>
      <Head>
        <title>Welcome to web-app!</title>
      </Head>
      <main className="app">
        <Component {...pageProps} />
      </main>
    </>
  );
}

export default CustomApp;
