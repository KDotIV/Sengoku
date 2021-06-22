/* eslint-disable react/jsx-props-no-spreading */
import Page from '../components/Pages';

export default function sengokuApp({ Component, pageProps }) {
  return (
    <Page>
      <Component {...pageProps} />
    </Page>
  );
}
