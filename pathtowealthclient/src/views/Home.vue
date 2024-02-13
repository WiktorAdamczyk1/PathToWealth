<template>
    <el-config-provider namespace="ep">
        <div class="flex main-container">
            <div w="full">
                <section id="welcome" style="padding: 0px 3px 0px 3px;">
                    <WelcomeSection />
                    <SectionNavigation />
                </section>
                <section id="about" class="basic-section">
                    <AboutPanel />
                </section>
                <section id="form" class="alternate-section">
                    <div>
                        <UserFinancialDataForm />
                    </div>
                </section>
                <section id="investment" class="basic-section">
                    <InvestmentPanel />
                    <InvestmentChart />
                </section>
                <section id="withdrawal" class="alternate-section">
                    <WithdrawalsPanel />
                    <WithdrawalsChart />
                </section>
                <section id="contact" class="basic-section-contact" style="padding: 16px 24px 16px 24px;">
                    <ContactSection />
                </section>
            </div>
        </div>
    </el-config-provider>
</template>

<script lang="ts">
import { ElButton, ElIcon } from 'element-plus';

export default {
    components: {
        ElButton,
        ElIcon,
    },
    methods: {
        scrollToSection(sectionId: string) {
            const section = document.getElementById(sectionId);
            if (section) {
                section.scrollIntoView({ behavior: 'smooth' });
            }
        },
    },
};
</script>
<style lang="scss">

.basic-section {
    background-color: #ffffff;
    padding: 2rem 3rem;
}

.dark .basic-section {
    background-color: #121212;
    padding: 2rem 3rem;
}

.basic-section-contact {
    padding: 2rem 3rem;
}

.dark .basic-section-contact {
    padding: 2rem 3rem;
}

.alternate-section {
  background-color: #eff7fd;
  padding: 2rem 3rem;
}

.dark .alternate-section {
  background-color: #181818;
}

.content-container {
    --rem: 16;
    --salmon: #F5A091;
    --lightblue: #A8DAF4;
    --lightgreen: #d2e3d2;
    --lightyellow: #FBFDD2;
    --lightpurple: #ABAEDB;
    --lighttheme: #e5eaf3;
    --darktheme: #181818;
    box-sizing: border-box;
    scroll-behavior: smooth;
    margin: 30px;
    border: 0;
    font: inherit;
    vertical-align: baseline;
    display: flex;
    flex-direction: column;
    border-radius: 2rem;
    padding: 4rem 3rem;
    justify-content: space-between;
    flex-shrink: 0;
    transition: margin-top 0.5s ease;
    position: relative;
    overflow: hidden;
    /* aspect-ratio: 1; */
    background-color: var(--lighttheme);
    translate: none;
    rotate: none;
    scale: none;
    transform: translate(0px, 0px);
    opacity: 1;
    visibility: inherit;
    box-shadow: inset 0px 2px 8px rgba(0, 0, 0, 0.2);
}

.dark .content-container {
    background-color: var(--darktheme);
    box-shadow: 0px -2px 8px rgba(255, 255, 255, 0.2);
}

.overlay-component {
    position: absolute;
    width: 400px;
    top: 700px;
    left: 50%;
    transform: translateX(-50%);
    height: 50px;
    z-index: 1;
    background-color: rgba(0, 0, 0, 0.2);
    backdrop-filter: blur(10px);
    border-radius: 99rem;
    border-width: 0px;
    box-shadow: inset 0px 2px 8px rgba(0, 0, 0, 0.2);
    display: flex;
    justify-content: space-between;

}

//Border animation (unused)
.animated-border {
  position: absolute;
  inset: 0;
  top: 10%;
  left: 0;
  height: 100%;
  background: conic-gradient(
    transparent 50deg,
    #ff4874 80deg,
    transparent 100deg
  );
  filter: blur(8px);
  border-radius: 2rem;
  transform-origin: center;
  animation: border-animation -0.64s linear 4s infinite;
  will-change: transform;
}

.wrapper {
  margin: 30px;
  padding: 4px;
  position: relative;
  overflow: hidden;
  background: linear-gradient(0deg, rgba(22, 4, 8, 0.5), rgba(22, 4, 8, 0.5)),
    linear-gradient(100.41deg, #ff5b37 0.52%, #ab0052 100.52%);
  border-radius: 2rem;
  box-shadow: 0 0 27px rgba(173, 40, 40, 0.48);
  display: block;
}

.content {
  position: relative;
  z-index: 1;
  background: linear-gradient(0deg, rgba(22, 4, 8, 0.96), rgba(22, 4, 8, 0.96)),
    linear-gradient(100.41deg, #ff5b37 0.52%, #ab0052 100.52%);
  border: 1px solid hsla(0, 0%, 100%, 0.1);
  border-radius: 2rem;
}

@keyframes border-animation {
  0% {
    transform: translateY(-25%) translateX(40%) rotate(0deg);
  }

  30% {
    transform: translateY(0) translateX(0) rotate(90deg) scaleX(0.7);
  }

  50% {
    transform: translateY(-25%) translateX(-40%) rotate(180deg);
  }

  70% {
    transform: translateY(0) translateX(0) rotate(270deg) scaleX(0.7);
  }

  to {
    transform: translateY(-25%) translateX(40%) rotate(1turn);
  }
}

</style>