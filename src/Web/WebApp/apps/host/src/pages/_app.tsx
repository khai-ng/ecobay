import { AppProps } from 'next/app';
import Head from 'next/head';
import './styles.css';
import '@shared/fonts/fontawesome/css/all.css';
import '@shared/fonts/garet/css/style.css';

function CustomApp({ Component, pageProps }: AppProps) {
  return (
    <>
      <Head>
        <title>ecobay</title>
      </Head>
      <main className="app app_container">
        <Component {...pageProps} />
      </main>
    </>
  );
}

export default CustomApp;
