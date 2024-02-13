<template>
    <div class="social-container">
        <a href="https://www.linkedin.com/" class="linkedin"><span class="link-text">LinkedIn</span></a>
        <a href="https://github.com/" class="github"><span class="link-text">Github</span></a>
    </div>
</template>

<style lang="scss" scoped>
.social-container {
    display: flex;
    flex-direction: row;
    align-items: center;
    justify-content: center;
    
    a {
        display: inline-block;
        width: fit-content;
        line-height: 40px;
        margin: 20px 0;
        text-decoration: none;
        text-align: center;
        font-size: 22px;
        font-weight: bold;
        padding: 0px 30px 0px 30px;
        
    }
}

.social-container a::before {
    background-color: transparent;
    bottom: 0;
    content: '';
    left: 0;
    position: fixed;
    right: 0;
    top: 20px;
    z-index: -1;
}

.social-container a i {
    margin-right: 15px;
}

.social-container a:hover {
    color: rgba(255, 255, 255, .6);
}

.linkedin:hover::before {
    background-color: #007bb6;
}

.github:hover::before {
    background-color: #6e5494;
}

.social-container a,
.social-container a::before {
    -webkit-transition: all .2s ease-in;
    -moz-transition: all .2s ease-in;
    -ms-transition: all .2s ease-in;
    -o-transition: all .2s ease-in;
    transition: all .2s ease-in;
}
</style>
