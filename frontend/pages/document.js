import Document, { Html, Head, NextScript, Main } from 'next/document';

export default class sengokuDocument extends Document {
  render() {
    return (
      <Html lang="en-us">
        <Head />
        <body>
          <Main />
          <NextScript />
        </body>
      </Html>
    );
  }
}
