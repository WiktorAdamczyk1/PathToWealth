<template>
    <section class="welcome-section">
        <div class="welcome-image-container">
            <div class="home col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <h3 id="resizing-h3" class="">
                    <span>
                        <h1 class="title">Path to </h1>
                        <div class="stage"> 
                            <div class="cubespinner">
                                <div class="face1">Happiness</div>
                                <div class="face2">Wealth</div>
                                <div class="face3">Security</div>
                                <div class="face4">Retirement</div>
                            </div>
                        </div>
                    </span>
                </h3>
            </div>
        </div>
    </section>
</template>

<style scoped>
.welcome-section img {
    width: 600px;
    height: auto;
    align-self: center;
}

.welcome-image-container {
    --rem: 16;
    --salmon: #F5A091;
    --lightblue: #A8DAF4;
    --lightgreen: #d2e3d2;
    --lightyellow: #FBFDD2;
    --lightpurple: #ABAEDB;
    min-height: 300px;
    height: 700px;
    box-sizing: border-box;
    display: flex;
    flex-direction: column;
    border-radius: 2rem;
    padding: 0rem 0rem;
    flex-shrink: 0;
    transition: margin-top 0.5s ease;
    position: relative;
    overflow: hidden;
    background-color: var(--lightgreen);
    transform: translate(0px, 0px);
    opacity: 1;
    visibility: inherit;
    background-image:  url("/mountains-7240579_1920.webp");
    background-repeat: no-repeat;
    background-size: 100% 1050px ;
}


body {
    margin: 0px;
    padding: 0px;
    font-family: sans-serif;
}

a:hover,
a:active {
    text-decoration: none;
}

.home {
    width: 100%;
}

.home:before {
    content: ' ';
    position: absolute;
    left: 0;
    right: 0;
    top: 0;
    bottom: 0;
}

.home h3 {
    position: absolute;
    font-size: 74px;
    text-align: left;
    color: #8ff08f;
    margin-left: 10%;
    font-weight: 700;
}

.stage {
    width: 400px;
    margin-top: 26px;
    height: 100px;
}

span {
  display: flex;
  flex-wrap: wrap;
  align-items: center;
  justify-content: flex-start;
  gap: 20px;
  overflow-wrap: break-word;
}

.cubespinner {
    -webkit-animation-name: spincube;
    -webkit-animation-timing-function: ease-in-out;
    -webkit-animation-iteration-count: infinite;
    -webkit-animation-duration: 8s;
    animation-name: spincube;

    animation-timing-function: ease-in-out;
    animation-iteration-count: infinite;
    animation-duration: 8s;
    -webkit-transform-style: preserve-3d;
    -moz-transform-style: preserve-3d;
    -ms-transform-style: preserve-3d;
    transform-style: preserve-3d;
    -webkit-transform-origin: 40px 40px 0;
    -moz-transform-origin: 40px 40px 0;
    -ms-transform-origin: 40px 40px 0;
    transform-origin: 40px 40px 0;
}

.cubespinner div {
    position: absolute;
    width: 400px;
    height: 80px;
    text-align: left;
    color: #931010;
    font-family: 'Raleway', sans-serif;
    font-size: 74px;
    font-weight: 700;
}

.cubespinner .face1 {
    color: #f1b50f;
    -webkit-transform: translateZ(40px);
    -moz-transform: translateZ(40px);
    -ms-transform: translateZ(40px);
    transform: translateZ(40px);
    backface-visibility: hidden;
}

.cubespinner .face2 {
    color: #8ff08f;
    -webkit-transform: rotateX(90deg) translateZ(40px);
    -moz-transform: rotateX(90deg) translateZ(40px);
    -ms-transform: rotateX(90deg) translateZ(40px);
    transform: rotateX(90deg) translateZ(40px);
    backface-visibility: hidden;
}

.cubespinner .face3 {
    color: #3d8eda;
    -webkit-transform: rotateX(180deg) translateZ(40px);
    -moz-transform: rotateX(180deg) translateZ(40px);
    -ms-transform: rotateX(180deg) translateZ(40px);
    transform: rotateX(180deg) translateZ(40px);
    backface-visibility: hidden;
}

.cubespinner .face4 {
    color: #fff;
    -webkit-transform: rotateX(270deg) translateZ(40px);
    -moz-transform: rotateX(270deg) translateZ(40px);
    -ms-transform: rotateX(270deg) translateZ(40px);
    transform: rotateX(270deg) translateZ(40px);
    backface-visibility: hidden;
}


/*Cubical Flipping or rotation*/
@-webkit-keyframes spincube {

    from,
    to {
        -webkit-transform: rotateX(0deg) rotateY(0deg) rotateZ(0deg);
    }

    15% {
        -webkit-transform: rotateX(90deg);
    }

    25% {
        -webkit-transform: rotateX(90deg);
    }

    40% {
        -webkit-transform: rotateX(180deg);
    }

    50% {
        -webkit-transform: rotateX(180deg);
    }

    65% {
        -webkit-transform: rotateX(270deg);
    }

    75% {
        -webkit-transform: rotateX(270deg);
    }

    95% {
        -webkit-transform: rotateX(360deg);
    }

    100% {
        -webkit-transform: rotateX(360deg);
    }
}

@keyframes spincube {

    from,
    to {
        -webkit-transform: rotateX(0deg);
        -moz-transform: rotateX(0deg);
        -ms-transform: rotateX(0deg);
        transform: rotateX(0deg);
    }

    15% {
        -webkit-transform: rotateX(90deg);
        -moz-transform: rotateX(90deg);
        -ms-transform: rotateX(90deg);
        transform: rotateX(90deg);
    }

    25% {
        -webkit-transform: rotateX(90deg);
        -moz-transform: rotateX(90deg);
        -ms-transform: rotateX(90deg);
        transform: rotateX(90deg);
    }

    40% {
        -webkit-transform: rotateX(180deg);
        -moz-transform: rotateX(180deg);
        -ms-transform: rotateX(180deg);
        transform: rotateX(180deg);
    }

    50% {
        -webkit-transform: rotateX(180deg);
        -moz-transform: rotateX(180deg);
        -ms-transform: rotateX(180deg);
        transform: rotateX(180deg);
    }

    65% {
        -webkit-transform: rotateX(270deg);
        -moz-transform: rotateX(270deg);
        -ms-transform: rotateX(270deg);
        transform: rotateX(270deg);
    }

    75% {
        -webkit-transform: rotateX(270deg);
        -moz-transform: rotateX(270deg);
        -ms-transform: rotateX(270deg);
        transform: rotateX(270deg);
    }

    90% {
        -webkit-transform: rotateX(360deg);
        -moz-transform: rotateX(360deg);
        -ms-transform: rotateX(360deg);
        transform: rotateX(360deg);
    }

    100% {
        -webkit-transform: rotateX(360deg);
        -moz-transform: rotateX(360deg);
        -ms-transform: rotateX(360deg);
        transform: rotateX(360deg);
    }
}</style>
