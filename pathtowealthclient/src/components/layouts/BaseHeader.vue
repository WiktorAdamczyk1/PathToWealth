<script lang="ts" setup>
import { toggleDark } from "~/composables";
import user from "../../stores/user";
</script>

<template>
  <el-menu :default-active="$route.path" :router="true" class="el-menu"
    style="display: flex; justify-content: space-between; border: 0px">
    <div>
      <router-link to="/" class="no-underline">
        <el-menu-item v-if="!isSmallScreen" class="el-menu-item" index="/">Path to Wealth</el-menu-item>
        <el-menu-item v-else class="el-menu-item" index="/">
          <img h="full" src="/LogoGreen.png" alt="Logo" />
        </el-menu-item>
      </router-link>
    </div>
    <div style="display: flex">
      <template v-if="!isSmallScreen">
        <router-link to="/account" class="no-underline" :class="{ 'p-disabled': !user.getters.isLoggedIn }">
          <el-menu-item class="el-menu-item" index="/account" :disabled="!user.getters.isLoggedIn">Account</el-menu-item>
        </router-link>
        <LoginDialog v-if="!user.getters.isLoggedIn" />
        <el-menu-item class="el-menu-item" v-else @click="user.logout()"> Sign out </el-menu-item>
        <el-menu-item class="el-menu-item" @click="toggleDark()">
          <button class="border-none w-full bg-transparent">
            <i class="icon" inline-flex i="dark:ep-moon ep-sunny" />
          </button>
        </el-menu-item>
      </template>
      <template v-else>
        <input id="burger" type="checkbox" class="checbox-hidden" v-model="isBurgerOpen" ref="burgerCheckbox"/>

        <label for="burger">
          <span></span>
          <span></span>
          <span></span>
        </label>
        <nav>
          <ul>
            <li>
              <router-link to="/account" class="no-underline" :class="{ 'p-disabled': !user.getters.isLoggedIn }">
                <el-menu-item class="el-menu-item" index="/account"
                  :disabled="!user.getters.isLoggedIn"><h style="text-align: center; width: 100%;">Account</h></el-menu-item>
              </router-link>
            </li>
            <li>
              <LoginDialog v-if="!user.getters.isLoggedIn" />
              <el-menu-item class="el-menu-item" v-else @click="user.logout()"><h style="text-align: center; width: 100%;"> Sign out </h></el-menu-item>
            </li>
            <li>
              <el-menu-item class="el-menu-item" style="" @click="toggleDark()">
                <button class="border-none w-full bg-transparent">
                  <i class="icon" inline-flex i="dark:ep-moon ep-sunny" />
                </button>
              </el-menu-item>
            </li>
          </ul>
        </nav>
      </template>
    </div>
  </el-menu>
</template>

<script lang="ts">
import { useDark } from "@vueuse/core";
import { ref, watch, watchEffect } from "vue";

const isDark = useDark();
const isSmallScreen = ref(false);

const SMALL_SCREEN_BREAKPOINT = 768;

watchEffect(() => {
  isSmallScreen.value = window.innerWidth < SMALL_SCREEN_BREAKPOINT;
});

window.addEventListener('resize', () => {
  isSmallScreen.value = window.innerWidth < SMALL_SCREEN_BREAKPOINT;
});

export default {
  data() {
    return {
      isBurgerOpen: false,
    };
  },
  watch: {
    isBurgerOpen(newVal) {
      if (newVal) {
        document.body.classList.add('no-scroll');
        window.scrollTo(0, 0);
      } else {
        document.body.classList.remove('no-scroll');
      }
    },
  },
};

</script>

<style lang="scss">

.no-scroll {
  overflow: hidden;
}

.el-menu {
  background-color: var(--ep-background-color);
  color: var(--ep-text-color);
  height: 56px;
  border: 0px;
  border-radius: 10px;
  padding: 0px 20px;
  margin: 10px;
  min-height: 50px;
  display: flex;
  justify-content: space-between;
  align-items: center;
  font-size: 1.3rem;
}

// .el-menu-shadow-light {
//   box-shadow: inset 0px 2px 8px rgba(0, 0, 0, 0.2);
// }

// .el-menu-shadow-dark {
//   box-shadow: 0px -2px 8px rgba(255, 255, 255, 0.2);
// }

.el-menu-item {
  color: #3d6b3c;
  text-align: center;
  border-radius: 99rem;
  transition: background-color 0.3s ease;
  height: 3rem;
  font-size: inherit;

  &:hover {
    background-color: #d2e3d2;
  }
}

.dark .el-menu-item {
  color: #6d9b6d;
  &:hover {
    background-color: #333b33;
  }
}

.ep-menu-item.is-active {
    color: var(--ep-menu-active-color);
}

.el-menu-item .icon {
  font-size: 1.3rem;
  color: #3d6b3c;
  transition: color 0.3s ease;
}

.dark .el-menu-item .icon {
  color: #6d9b6d;
}

// Hamburger menu styles
@media (max-width: 768px) {
  
  .el-menu {
    display: none;
  }

  .checbox-hidden {
    display: none;
  }

  $blackColor: #020304;
  $whiteColor: #f5f5f5;

  input+label {
    position: absolute;
    top: 18px;
    left: 87.5vw;
    height: 20px;
    width: 15px;
    z-index: 5;

    span {
      position: absolute;
      width: 100%;
      height: 2px;
      top: 50%;
      margin-top: -1px;
      left: 0;
      background-color: #3d6b3c;
      display: block;
      transition: .5s;
    }

    span:first-child {
      top: 3px;
    }

    span:last-child {
      top: 16px;
    }
  }

  .dark input+label {
    span {
      background-color: #6d9b6d;
    }
  }

  label:hover {
    cursor: pointer;
  }

  input:checked+label {
    span {
      opacity: 0;
      top: 50%;
    }

    span:first-child {
      opacity: 1;
      transform: rotate(405deg);
    }

    span:last-child {
      opacity: 1;
      transform: rotate(-405deg);
    }
  }

  input~nav {
    background: $whiteColor;
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    z-index: 3;
    transition: 0s;
    transition-delay: 0s;
    overflow: hidden;

    >ul {
      text-align: center;
      align-items: center;
      position: absolute;
      top: 35%;
      left: 20%;
      right: 20%;
      list-style-type: none;

      >li {
        opacity: 0;
        transition: 0s;
        transition-delay: 0s;
      }
    }
  }

  .dark input~nav {
    background: $blackColor;
    color: $whiteColor;

    >ul {
      >li {
        >a {
          color: $whiteColor;
        }
      }
    }
  }

  input:checked~nav {
    height: 100%;
    transition-delay: 0s;

    >ul {
      >li {
        opacity: 1;
        transition-delay: 0s;
        >a {
          color: $blackColor;
        }
      }
    }
  }

  .dark input:checked~nav {
    >ul {
      >li {
        >a {
          color: $whiteColor;
        }
      }
    }
  }
}
</style>
