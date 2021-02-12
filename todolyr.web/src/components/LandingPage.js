import '../components/LandingPage.css';

function LandingPage() {
    return(
        <body className="text-center">
            <div className="cover-container d-flex h-100 p-3 mx-auto flex-column">
                <main role="main" className="inner cover">
                    <h1 className="cover-heading">Todolyr</h1>
                    <p className="lead">The world's most over engineered Todo List</p>
                    <button className="btn btn-primary">Sign Up</button>
                    <button className="btn btn-primary">Login</button>
                </main>
            </div>
        </body>       
    );
}

export default LandingPage;