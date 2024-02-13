<template>
  <div class="blog-cards-container">
    <div class="blog-card">
      <div class="meta">
        <div class="photo" style="background-image: url(blog-card1.webp)"></div>
      </div>
      <div class="description">
        <h1>Simply Invest</h1>
        <h2>Inspired by "The Simple Path to Wealth" - JL Collins</h2>
        <p>Follow the simple and effective strategy for accruing wealth, presented in the acclaimed book "The Simple Path to Wealth" by JL Collins. Reach financial freedom by mastering the basics of personal finance - spend less than you earn, avoid debt, and invest in stock index funds. By following the principles outlined in the book, you can take control of your finances and build a secure future for yourself. Collins demystifies the complexities of stocks and bonds, offering a simple approach to making money work for you, not against you.
        </p>
      </div>
    </div>
    <div class="blog-card alt">
      <div class="meta">
        <div class="photo" style="background-image: url(blog-card2.webp)"></div>
      </div>
      <div class="description">
        <h1>Plan For Tommorow</h1>
        <h2>Research your future with our tools</h2>
        <p>Take control of your financial future with our powerful compound calculator. This tool allows you to visualize the growth of your investments over time, empowering you to make informed decisions about your retirement savings. Simply input your predicted return rates for your chosen stock and bond funds, and the calculator will show you how your money can grow using the power of compound interest. Create an account to save your data and track your progress towards your financial goals. Our compound calculator is a valuable resource for anyone looking to plan for their retirement and build a secure future. Start planning today!
        </p>
      </div>
    </div>
  </div>
</template>

<style lang="scss">

* {
  box-sizing: border-box;
}

$color_white: #fff;
$color_prime: #8ff08f;
$color_grey: #dddddd;
$color_grey_dark: #777777;
$color_light_bg: #eff7fd;
$color_dark_bg: #181818;

.blog-cards-container {
  display: flex;
  flex-wrap: wrap;
  justify-content: space-between;
}

.blog-card {
  display: flex;
  flex-direction: column;
  margin: 1rem auto;
  box-shadow: 0 3px 7px -1px rgba(#000, .1);
  margin-bottom: 1.6%;
  background: $color_light_bg;
  line-height: 1.4;
  font-family: sans-serif;
  border-radius: 1rem;
  overflow: hidden;
  z-index: 0;
  box-shadow: inset 0px 2px 8px rgba(0, 0, 0, 0.2);
  a {
    color: inherit;
    &:hover {
      color: $color_prime;
    }
  }
  &:hover {
    .photo {
      transform: scale(1.3) rotate(3deg);
    }
  }
  .meta {
    position: relative;
    z-index: 0;
    height: 200px;
  }
  .photo {
    position: absolute;
    top: 0;
    right: 0;
    bottom: 0;
    left: 0;
    background-size: cover;
    background-position: center;
    transition: transform .2s;
  }

  .description {
    padding: 1rem;
    background: $color_light_bg;
    position: relative;
    z-index: 1;
    h1,
    h2 {
      font-family: Poppins, sans-serif;
    }
    h1 {
      line-height: 1;
      margin: 0;
      font-size: 1.7rem;
    }
    h2 {
      font-size: 1rem;
      font-weight: 300;
      text-transform: uppercase;
      color: $color_grey_dark;
      margin-top: 5px;
    }
    .read-more {
      text-align: right;
      a {
        color: $color_prime;
        display: inline-block;
        position: relative;
        &:after {
          content: "\f061";
          font-family: FontAwesome;
          margin-left: -10px;
          opacity: 0;
          vertical-align: middle;
          transition: margin .3s, opacity .3s;
        }

        &:hover:after {
          margin-left: 5px;
          opacity: 1;
        }
      }
    }
  }
  p {
    position: relative;
    margin: 1rem 0 0;
    &:first-of-type {
      margin-top: 1.25rem;
      &:before {
        content: "";
        position: absolute;
        height: 5px;
        background: $color_prime;
        width: 35px;
        top: -0.75rem;
        border-radius: 3px;
      }
    }
  }
  &:hover {
    .details {
      left: 0%;
    }
  }


  @media (min-width: 640px) {
    flex-direction: row;
    max-width: 700px;
    .meta {
      flex-basis: 40%;
      height: auto;
    }
    .description {
      flex-basis: 60%;

    }
  }
}

.dark .blog-card{
  background-color: $color_dark_bg;
  color: $color_white;
  .photo {
    background-color: $color_dark_bg;
  }
  .description {
    background-color: $color_dark_bg;
    h2 {
      color: $color_grey;
    }
  }
  a {
    color: $color_prime;
  }
}

</style>