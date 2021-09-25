import './App.css';
import config from './config.json';
import { GoogleLogin } from 'react-google-login';


function App() {

  const googleResponse = (response) => {
    const tokenBlob = new Blob([JSON.stringify({ tokenId: response.tokenId }, null, 2)], { type: 'application/json' });
    const options = {
      method: 'POST',
      body: tokenBlob,
      mode: 'cors',
      cache: 'default'
    };
    fetch(config.GOOGLE_AUTH_CALLBACK_URL, options)
      .then(r => {
        r.json().then(res => {
          console.log(res);
        });
      })
  };

  return (
    <div className="App">
      <header className="App-header">
        <GoogleLogin
          clientId={config.GOOGLE_CLIENT_ID}
          buttonText="Google Login"
          onSuccess={googleResponse}
          onFailure={googleResponse}
        />
      </header>
    </div>
  );
}

export default App;
